import { Component, OnInit } from '@angular/core';
import { ThemePalette } from '@angular/material/core';

@Component({
  selector: 'app-login-register-component',
  templateUrl: './login-register.component.html',
  styleUrls: ['./login-register.component.css']
})
export class LoginRegisterComponent implements OnInit {
  background: ThemePalette = undefined;
  constructor() {}

  ngOnInit(): void {}

  toggleBackground(): void {
    this.background = this.background ? undefined : 'primary';
  }
}
