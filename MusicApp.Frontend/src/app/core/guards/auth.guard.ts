import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '@core/services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  
   const authService = inject(AuthService);
   const router = inject(Router);

   console.log('router--', route, '==',state, authService.isLoggedIn());

   if(authService.isLoggedIn() && route.routeConfig?.path === 'auth/login'){
      router.navigate(['/home']);
      return false;
   } else if (!authService.isLoggedIn() && route.routeConfig?.path !== 'auth/login') {
      // return router.parseUrl('/login');
      router.navigate(['/auth/login']); // Redirect to the login page
      return false;
   }

   return true;

};
