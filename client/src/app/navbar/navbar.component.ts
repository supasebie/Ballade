import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  model: any = {};

  constructor(
    public accountService: AccountService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
