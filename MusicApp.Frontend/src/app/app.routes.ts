import { Routes } from '@angular/router';
import { authGuard } from '@core/guards/auth.guard';
import { publicAuthGuard } from '@core/guards/public.auth.guard';

export const routes: Routes = [
    {
        path: 'auth',
        loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule),
        canActivate: [publicAuthGuard]
      },
      {
        path: '',
        loadChildren: () => import('./modules/main/main.module').then(m => m.MainModule),
        canActivate: [authGuard]
      },
      { path: '', redirectTo: '/auth/login', pathMatch: 'full' }, // Redirect to login by default
      { path: '**', redirectTo: '/auth/login' },
];
