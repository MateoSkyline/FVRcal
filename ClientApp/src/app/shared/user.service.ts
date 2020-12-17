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
      Validators.email,
      Validators.required
    ]],
    UserName: ['', Validators.required],
    Passwords: this.formBuilder.group({
      Password: ['', Validators.compose([
        Validators.required,
        UserService.patternValidator(new RegExp('[0-9]'), { hasNumber: true }),
        UserService.patternValidator(new RegExp('[A-Z]'), { hasCapitalCase: true }),
        UserService.patternValidator(new RegExp('[a-z]'), { hasSmallCase: true })
      ])],
      ConfirmPassword: ['', Validators.required]
    }, { validator: this.comparePasswords })
  });

  loginFormModel = this.formBuilder.group({
    Email: ['', [
      Validators.email,
      Validators.required      
    ]],
    Password: ['', Validators.required]
  })

  forgotPasswordFormModel = this.formBuilder.group({
    Email: ['', [
      Validators.email,
      Validators.required
    ]]
  })

  editUserFormModel = this.formBuilder.group({
    FirstName: ['', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(60)
    ]],
    LastName: ['', [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(60)
    ]],
    UserName: ['', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(30)
    ]],
    Passwords: this.formBuilder.group({
      Password: ['', Validators.compose([
        UserService.patternValidator(new RegExp('[0-9]'), { hasNumber: true }),
        UserService.patternValidator(new RegExp('[A-Z]'), { hasCapitalCase: true }),
        UserService.patternValidator(new RegExp('[a-z]'), { hasSmallCase: true })
      ])],
      ConfirmPassword: ['']
    }, { validator: this.comparePasswords }),
    OldPassword: [''],
    Email: ['', [
      Validators.email,
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(120)
    ]],
    PhoneNumber: [''],
    TwoFactor: ['']
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
      Password: this.registerFormModel.value.Passwords.Password
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

  forgotPassword() {
    var body = {
      Email: this.forgotPasswordFormModel.value.Email
    }
    return this.http.post(this.baseUrl + 'api/Account/ForgotPassword', body);
  }

  getUserProfile() {
    return this.http.get(this.baseUrl + 'api/UserProfile');
  }

  getUserEdit() {
    return this.http.get(this.baseUrl + 'api/Account/UserEdit');
  }

  updateUserEdit() {
    var body = {
      FirstName: (this.editUserFormModel.controls['FirstName'].touched ? this.editUserFormModel.value.FirstName : null),
      LastName: (this.editUserFormModel.controls['LastName'].touched ? this.editUserFormModel.value.LastName : null),
      UserName: (this.editUserFormModel.controls['UserName'].touched ? this.editUserFormModel.value.UserName : null),
      PhoneNumber: (this.editUserFormModel.controls['PhoneNumber'].touched ? this.editUserFormModel.value.PhoneNumber : null),
      Email: (this.editUserFormModel.controls['Email'].touched ? this.editUserFormModel.value.Email : null),              
      TwoFactorEnabled: this.editUserFormModel.value.TwoFactor,
      PasswordHash: (this.editUserFormModel.get('Passwords.Password').touched ? this.editUserFormModel.value.Passwords.Password : null),
      OldPassword: (this.editUserFormModel.controls['OldPassword'].touched ? this.editUserFormModel.value.OldPassword : null),
    }
    return this.http.post(this.baseUrl + 'api/Account/UserEdit', body);
  }

  verifyEmailRegister(id, token) {
    var body = {
      userID: id,
      token: token
    }
    return this.http.post(this.baseUrl + 'api/Account/VerifyEmail', body);
  }

  verifyEmailEdit(id, mail, token) {
    var body = {
      userID: id,
      newMail: mail,
      token: token
    }
    return this.http.post(this.baseUrl + 'api/Account/VerifyChangedEmail', body);
  }
}
