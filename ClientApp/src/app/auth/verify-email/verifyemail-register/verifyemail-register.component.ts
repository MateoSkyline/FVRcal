import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../shared/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-verifyemail-register',
  templateUrl: './verifyemail-register.component.html',
  styleUrls: ['./verifyemail-register.component.css']
})
export class VerifyemailRegisterComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute, public service: UserService, private snackBar: MatSnackBar, private router: Router) { }

  loaded: boolean = false;
  success: boolean = false;

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe(params => {
      const id = params['userID'];
      const token = params['token'];
      this.service.verifyEmailRegister(id, token).subscribe(
        (res: any) => {
          this.snackBar.open("Your email address is now verified!", "OK", { duration: 5000, });
          this.loaded = true;
          this.success = true;
          setTimeout(() => {
            this.router.navigateByUrl('/auth/login');
          }, 4000);
        },
        err => {
          console.error(err);
          this.loaded = true;
        }
      );
    })
  }

}
