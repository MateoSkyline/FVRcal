import { Injectable, Inject } from '@angular/core';
import { FormBuilder, Validators, FormGroup, ValidationErrors, ValidatorFn, AbstractControl } from '@angular/forms';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private formBuilder: FormBuilder, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router) {
    baseUrl = baseUrl;
  }

  registerFormModel = this.formBuilder.group({
    FirstName: ['', Validators.required],
    LastName: ['', Validators.required],
    Email: ['', [
      Validators.email, Validators.required
    ]],
    UserName: ['', Validators.required],
    Password: ['', Validators.compose([
      Validators.required,
      UserService.patternValidator(new RegExp('[0-9]'), { hasNumber: true }),
      UserService.patternValidator(new RegExp('[A-Z]'), { hasCapitalCase: true }),
      UserService.patternValidator(new RegExp('[a-z]'), { hasSmallCase: true })
    ])],
    ConfirmPassword: ['', Validators.required]
  });

  loginFormModel = this.formBuilder.group({
    Email: ['', [
      Validators.required,
      Validators.email
    ]],
    Password: ['', Validators.required]
  })

  comparePasswords(formBuilder: FormGroup) {
    let confirmPassword = formBuilder.get('ConfirmPassword');
    if (confirmPassword.errors == null || 'passwordMismatch' in confirmPassword.errors) {
      if (formBuilder.get('Password').value != confirmPassword.value)
        confirmPassword.setErrors({ passwordMismatch: true });
      else
        confirmPassword.setErrors(null);
    }
  }

  static patternValidator(regex: RegExp, error: ValidationErrors): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
      if (!control.value) {
        return null;
      }
      const valid = regex.test(control.value);

      return valid ? null : error;
    };
  }

  register() {
    var body = {
      FirstName: this.registerFormModel.value.FirstName,
      LastName: this.registerFormModel.value.LastName,
      Email: this.registerFormModel.value.Email,
      UserName: this.registerFormModel.value.UserName,
      Password: this.registerFormModel.value.Password
    };
    return this.http.post(this.baseUrl + 'api/Account/Register', body);
  }

  login() {
    var body = {
      Email: this.loginFormModel.value.Email,
      Password: this.loginFormModel.value.Password
    }
    return this.http.post(this.baseUrl + 'api/Account/Login', body);
  }

  logout() {
    this.router.navigateByUrl('/');
    return localStorage.removeItem('token');
  }

  getUserProfile() {
    return this.http.get(this.baseUrl + 'api/UserProfile');
  }
}
