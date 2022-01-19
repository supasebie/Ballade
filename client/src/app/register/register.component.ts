import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidatorFn,
  Validators
} from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { UserRegister } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter<boolean>();
  model: any = {};
  registerForm: FormGroup;
  validationErrors: string[] = [];

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private accountService: AccountService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null
        : { isMatching: true };
    };
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male', Validators.required],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: [
        '',
        [Validators.required, Validators.minLength(4), Validators.maxLength(8)]
      ],
      confirmPassword: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(8),
          this.matchValues('password')
        ]
      ]
    });
    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    });
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe(
      (response) => {
        this.router.navigateByUrl('/members');
      },
      (error) => {
        this.validationErrors = error;
      }
    );
    this.cancel();
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
