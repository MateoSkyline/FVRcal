import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';
import { error } from 'protractor';
import { Router } from '@angular/router'

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public acc: Account[];

  registerForm;
  passwordComplex: number = 0;
  loaded: boolean = true;
  registerStatus: number = -1;
  lastEmailUsed: string = "123xyz987abc@qwe.mnb";
  databaseProblem: boolean = false;


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private formBuilder: FormBuilder, private router: Router) {
    this.registerForm = this.formBuilder.group({
      firstname: '',
      lastname: '',
      email: '',
      username: '',
      password: '',
      password2: ''
    })
  }

  ngOnInit() {

  }

  RegisterAccount(userData) {
    this.loaded = false;
    this.http.post(this.baseUrl + 'RegisterAccount', userData).subscribe(result => {
      if (!result) {
        console.log("Register successful!");
        this.loaded = true;
        this.router.navigate(['/counter']);
      }
      else if (result == 1) {
        console.log("Register failed. Email problem.");
        this.lastEmailUsed = userData.email;
        this.loaded = true;
      }
      else if (result == 2) {
        console.log("Register failed. Database problem.");
        this.databaseProblem = true;
        this.loaded = true;
      }
    }, error => console.error(error));
  }

  checkPasswordStrength(pass) {
    let score = 0;
    // award every unique letter until 5 repetitions  
    let letters = {};
    for (let i = 0; i < pass.length; i++) {
      letters[pass[i]] = (letters[pass[i]] || 0) + 1;
      score += 5.0 / letters[pass[i]];
    }
    // bonus points for mixing it up  
    let variations = {
      digits: /\d/.test(pass),
      lower: /[a-z]/.test(pass),
      upper: /[A-Z]/.test(pass),
      nonWords: /\W/.test(pass),
    };

    let variationCount = 0;
    for (let check in variations) {
      variationCount += (variations[check]) ? 1 : 0;
    }
    score += (variationCount - 1) * 10;
    this.passwordComplex = Math.trunc(score);
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
