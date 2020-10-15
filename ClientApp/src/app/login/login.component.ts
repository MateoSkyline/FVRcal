import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { resolveAny } from 'dns';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: any;
  loaded: boolean = true;
  databaseProblem: boolean = false;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private formBuilder: FormBuilder, private router: Router) {
    this.loginForm = this.formBuilder.group({
      email: '',
      password: ''
    })
  }

  ngOnInit() {
    if (localStorage.getItem('token') != null) {
      this.router.navigateByUrl('/');
    }
  }

  Login(userData) {
    this.loaded = false;
    this.http.post(this.baseUrl + 'api/Account/Login', userData).subscribe((result: any) => {
      console.log(result);
      localStorage.setItem('token', result.token);
      this.router.navigateByUrl('/');
    }, error => {
        if (error.status == 400) {
          console.error("Incorrect username or password");
        }
        else
          console.error(error);
    });
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
