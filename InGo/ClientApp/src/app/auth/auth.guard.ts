import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, Route, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '../start-page/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate  {

  constructor(private router: Router, private account: AccountService){

  }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
      if (localStorage.getItem('token') != null){
        let roles = next.data['permittedRoles'] as Array<string>;
        if(roles){
          if(this.account.roleMatch(roles)) return true;
          else{
            this.router.navigate(['noaccess']);
            return false;
          }
        }
        return true;
      }
      else {
        this.router.navigateByUrl('login');
        return false;
      }
      
    }
  
}
