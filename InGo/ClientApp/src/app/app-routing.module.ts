import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserAboutPanelComponent } from './user-about-panel/user-about-panel.component';
import { AuthGuard } from './auth/auth.guard';
import { ListOfFaqComponent } from './list-of-faq/list-of-faq.component';
import { PostComponent } from './post/post.component';
import { StartPageComponent } from './start-page/start-page.component';
import { AddPostFormComponent } from './add-post-form/add-post-form.component';
import { LoginComponent } from './start-page/authorization/login/login.component';
import { RegistrationComponent } from './start-page/authorization/registration/registration.component';
import { MainPageBodyComponent } from './main-page-body/main-page-body.component';
import { NoaccsessComponent } from './noaccsess/noaccsess.component';
import { AboutSitePageComponent } from './about-site-page/about-site-page.component';

const appRoutes: Routes = [
  {path: '', redirectTo: '/posts', pathMatch: 'full', canActivate: [AuthGuard] },
  {path: 'login', redirectTo: '/authorization/registration', pathMatch: 'full' },
  {
    path: 'authorization',  component: StartPageComponent,
    children: [
    { path: 'registration', component: RegistrationComponent} ,
    { path: 'login', component: LoginComponent }
   ]
},
  {path: 'addpost', component: AddPostFormComponent, canActivate: [AuthGuard]},
  {path: 'posts', component: MainPageBodyComponent, canActivate: [AuthGuard] },
  {path: 'post/:id', component: PostComponent, canActivate: [AuthGuard] },
  {path: 'faq', component: ListOfFaqComponent, canActivate: [AuthGuard]},
  {path: 'user/:id', component: UserAboutPanelComponent, canActivate: [AuthGuard]},
  {path: 'noaccess', component: NoaccsessComponent},
  {path:  'about', component: AboutSitePageComponent}
];

//добавить data :{permittedRoles:['Admin']} к атрибутамЮ компонент который будет доступен только для определенной роли

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }



