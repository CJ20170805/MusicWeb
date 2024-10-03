import { Routes, RouterModule } from "@angular/router";
import { HomeComponent } from "./page/home/home.component";
import { AboutComponent } from "./page/about/about.component";
import { NgModule } from "@angular/core";
import { ContentLayoutComponent } from "@layout/content-layout/content-layout.component";

const routes: Routes = [
    {
        path: '',
        component: ContentLayoutComponent,
        children: [
          { path: '', component: HomeComponent },
          { path: 'about', component: AboutComponent },
          { path: '', redirectTo: 'home', pathMatch: 'full' }, 
          { path: '**', component: HomeComponent },
        ]
      }
]
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class MainRoutingModule { }
