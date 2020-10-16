import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../shared/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  constructor(private router: Router, private service: UserService, private snackBar: MatSnackBar) { }
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  onLogout() {
    this.service.logout();
    this.snackBar.open("You just signed out.", "Thanks!", {duration: 2000,});
  }
}
