import { Routes, RouterModule } from "@angular/router";
import { LoginComponent } from "./page/login/login.component";
import { RegisterComponent } from "./page/register/register.component";
import { NgModule } from "@angular/core";
import { AuthLayoutComponent } from "@layout/auth-layout/auth-layout.component";

const routes: Routes = [
    {
        path: '',
        component: AuthLayoutComponent,
        children: [
          { path: 'login', component: LoginComponent },
          { path: 'register', component: RegisterComponent },
          { path: '', redirectTo: '/auth/login', pathMatch: 'full' }, 
          { path: '**', component: LoginComponent },
        ]
      }
]
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AuthRoutingModule { }
