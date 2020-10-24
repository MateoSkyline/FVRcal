import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserService } from '../../../shared/user.service';

@Component({
  selector: 'app-verifyemail-useredit',
  templateUrl: './verifyemail-useredit.component.html',
  styleUrls: ['./verifyemail-useredit.component.css']
})
export class VerifyemailUsereditComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute, public service: UserService, private snackBar: MatSnackBar, private router: Router) { }

  loaded: boolean = false;
  success: boolean = false;

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe(params => {
      const id = params['userID'];
      const email = params['newMail'];
      const token = params['token'];
      this.service.verifyEmailEdit(id, email, token).subscribe(
        (res: any) => {
          this.snackBar.open("Your new email address is verified!", "OK", { duration: 5000, });
          this.loaded = true;
          this.success = true;
          setTimeout(() => {
            this.router.navigateByUrl('/');
          }, 4000);
        },
        err => {
          console.error(err);
          this.loaded = true;
        }
      )
    })
  }

}
