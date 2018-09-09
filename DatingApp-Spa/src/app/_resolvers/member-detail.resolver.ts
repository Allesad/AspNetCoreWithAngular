import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/User';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MemberDetailResolver implements Resolve<User> {
  constructor(
    private userService: UserService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(root: ActivatedRouteSnapshot): Observable<User> {
    return this.userService.getUser(root.params['id'])
        .pipe(
            catchError(err => {
                console.error(err);
                this.alertify.error('Problem retreiving data');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
  }
}
