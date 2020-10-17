import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';
import { error } from 'protractor';
import { Router } from '@angular/router'
import { UserService } from '../../shared/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public acc: Account[];

  loaded: boolean = true;
  lastEmailUsed: string = "123xyz987abc@qwe.mnb";
  lastLoginUsed: string = "";


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private formBuilder: FormBuilder, private router: Router, public service: UserService, private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.service.registerFormModel.reset();
  }

  RegisterAccount() {
    this.loaded = false;
    this.service.register().subscribe(
      (res: any) => {
        if (res.succeeded) {
          this.service.registerFormModel.reset();
          this.router.navigateByUrl('/login');
          this.snackBar.open("You have just created an account! Now you can log in.", "OK", { duration: 5000, });
          this.loaded = true;
        } else {
          res.errors.forEach(element => {
            switch (element.code) {
              case 'DuplicateEmail':
                this.snackBar.open("This Email is already assigned to another account.", "OK", { duration: 5000, });
                this.lastEmailUsed = this.service.registerFormModel.controls['Email'].value;
                this.loaded = true;
                break;
              case 'DuplicateUserName':
                this.snackBar.open("This Username is already taken.", "OK", { duration: 5000, });
                this.lastLoginUsed = this.service.registerFormModel.controls['UserName'].value;
                this.loaded = true;
                break;
              default:
                this.snackBar.open("The registration has failed.", "OK", { duration: 5000, });
                console.error("Error : " + element.code);
                this.loaded = true;
                break;
            }
          })
        }
      },
      err => {
        console.error(err);
      }
    );
  }
}

interface Account {
  FirstName: string;
  LastName: string;
  Email: string;
  UserName: string;
  UserCode: string;
  PasswordHash: string;
  SecuritySalt: string;
}
