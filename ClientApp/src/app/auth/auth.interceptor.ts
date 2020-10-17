import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { tap } from "rxjs/operators";
import { Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private router: Router, private snackBar: MatSnackBar) {

  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (localStorage.getItem('token') != null) {
      const clonedReq = req.clone({
        headers: req.headers.set('Authorization', 'Bearer ' + localStorage.getItem('token'))
      });
      console.log(clonedReq);
      return next.handle(clonedReq).pipe(
        tap(
          succ => { },
          err => {
            if (err.status == 401) {
              this.snackBar.open("Your session has expired.", "OK", { duration: 5000, });
              localStorage.removeItem('token');
              this.router.navigateByUrl('/auth/login');
            }
          }
        )
      )
    }
    else
      return next.handle(req.clone());
  }
}
