import { Injectable, Inject } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private fb: FormBuilder, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    baseUrl = baseUrl;
  }

  readonly BaseURI = this.baseUrl;

  login(formData) {
    return this.http.post(this.BaseURI + 'api/Account/Login', formData);
  }

  getUserProfile() {
    console.log(this.BaseURI);
    return this.http.get(this.BaseURI + 'api/UserProfile');
  }

  logout() {
    return localStorage.removeItem('token');
  }
}
