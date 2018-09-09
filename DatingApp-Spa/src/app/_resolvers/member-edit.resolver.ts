import { Injectable } from '@angular/core';
import { Resolve, Router } from '@angular/router';
import { User } from '../_models/User';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<User> {
  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(): Observable<User> {
    return this.userService.getUser(this.authService.decodedToken.nameid)
        .pipe(
            catchError(err => {
                console.error(err);
                this.alertify.error('Problem retreiving your profile data');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
  }
}
