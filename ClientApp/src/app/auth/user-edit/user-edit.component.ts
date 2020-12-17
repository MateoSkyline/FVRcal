import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../shared/user.service';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {

  loaded: boolean = true;
  emailConfirmed: boolean;
  lastEmailUsed: string = "fvrcal.noreply@gmail.com";

  constructor(private router: Router, public service: UserService, private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.service.editUserFormModel.reset();
    this.getUserData();
  }

  getUserData() {
    this.loaded = false;
    this.service.getUserEdit().subscribe(
      (res: any) => {
        this.setFormModel(res);
        this.emailConfirmed = res.emailConfirmed;
        this.loaded = true;
      },
      (err: any) => {
        console.error(err);
        this.loaded = true;
      }
    )
  }

  updateUserData() {
    this.loaded = false;
    this.service.updateUserEdit().subscribe(
      (res: any) => {
        this.snackBar.open("Your user data has been successfully updated!", "OK", { duration: 5000, });
        this.loaded = true;
      },
      (err: any) => {
        console.error(err);
        this.snackBar.open("An error occured while trying to update your data.", "OK", { duration: 5000, });
        this.loaded = true;
      }
    )
  }

  setFormModel(userData) {
    this.service.editUserFormModel.patchValue({
      FirstName: userData.firstName,
      LastName: userData.lastName,
      Email: userData.email,
      UserName: userData.userName,
      PhoneNumber: userData.phoneNumber,
      TwoFactor: userData.twoFactorEnabled
    });
  }

}
