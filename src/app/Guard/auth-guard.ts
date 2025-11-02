import { Injectable } from '@angular/core';
import { 
  CanActivate, 
  CanActivateChild, 
  CanMatch, 
  Route, 
  UrlSegment, 
  ActivatedRouteSnapshot, 
  RouterStateSnapshot, 
  Router 
} from '@angular/router';
import { Observable } from 'rxjs';
import { LoginService } from '../services/login-service'; // Adjust the path if needed

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanActivateChild, CanMatch {

  constructor(private service: LoginService, private router: Router) {}

  // Helper method for redirection
  private redirectToLogin() {
    this.router.navigate(['/login']);
  }

  private checkAccess(allowedRoles?: string[]): boolean {
    const userRole = this.service.getUserRole();

    // If no user role, force login
    if (!userRole) {
      this.redirectToLogin();
      return false;
    }

    // If roles are defined in route data and user role not allowed → block
    if (allowedRoles && !allowedRoles.includes(userRole)) {
      this.redirectToLogin();
      return false;
    }

    return true;
  }

  canMatch(route: Route, segments: UrlSegment[]): boolean | Observable<boolean> | Promise<boolean> {
    const allowedRoles = route.data?.['roles'] as string[];
    return this.checkAccess(allowedRoles);
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const allowedRoles = route.data['roles'] as string[];
    return this.checkAccess(allowedRoles);
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const allowedRoles = childRoute.data['roles'] as string[];
    return this.checkAccess(allowedRoles);
  }
}
