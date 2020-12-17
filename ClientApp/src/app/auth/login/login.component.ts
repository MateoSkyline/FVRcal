import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { resolveAny } from 'dns';
import { UserService } from '../../shared/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { ForgotPasswordDialogComponent } from './forgotPasswordDialog/forgotPasswordDialog.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loaded: boolean = true;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private formBuilder: FormBuilder,
    private router: Router,
    public service: UserService,
    private snackBar: MatSnackBar,
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this.service.loginFormModel.reset();
    if (localStorage.getItem('token') != null) {
      this.router.navigateByUrl('/');
    }
  }

  Login() {
    this.loaded = false;
    this.service.login().subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token);
        this.router.navigateByUrl('/');
        this.snackBar.open("You have successfully logged in.", "OK", { duration: 5000, });
        this.loaded = true;
      },
      err => {
        if (err.status == 400) {
          this.snackBar.open("Incorrect username or password.", "OK", { duration: 5000, });
          this.loaded = true;
        } else {
          this.snackBar.open("An error occured. Try again.", "OK", { duration: 5000, });
          console.error(err);
          this.loaded = true;
        }
      }
    )
  }

  forgotPassword() {
    const dialogRef = this.dialog.open(ForgotPasswordDialogComponent, {
      width: '250px'
    })

    dialogRef.afterClosed().subscribe(result => {
      this.loaded = false;
      this.service.forgotPassword().subscribe(
        (res: any) => {
          this.snackBar.open("A password reset link has been sent to your email address.", "OK", { duration: 5000, });
          this.loaded = true;
        },
        err => {
          this.snackBar.open("An error occured. Try again.", "OK", { duration: 5000, });
          console.error(err);
          this.loaded = true;
        }
      )
    })
  }

}

interface Account {
  user_id: number;
  firstname: string;
  lastname: string;
  email: string;
  username: string;
  usercode: string;
  password: string;
  salt: string;
  permissions: string;
}
