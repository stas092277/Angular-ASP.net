import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { LMarkdownEditorModule } from 'ngx-markdown-editor';

import { AppComponent } from './app.component';
import { StartPageComponent } from './start-page/start-page.component';
import { PopularTagCloudComponent } from './main-page-body/popular-tag-cloud/popular-tag-cloud.component';
import { CategorySelectorComponent } from './main-page-body/category-selector/category-selector.component';
import { PostComponent } from './post/post.component';
import { FormEditCommentComponent } from './post/form-edit-comment/form-edit-comment.component';
import { CommentComponent } from './post/comment/comment.component';
import { ListOfFaqComponent} from './list-of-faq/list-of-faq.component';
import { AddPostFormComponent} from './add-post-form/add-post-form.component';
import { AddPostButtonComponent} from './add-post-button/add-post-button.component';
import { FeedPostsComponent } from './feed-posts/feed-posts.component';
import { FooterComponent } from './shared/layout/footer/footer.component';
import { UserAboutPanelComponent} from './user-about-panel/user-about-panel.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AccountService } from './start-page/account.service';
import { AuthInterceptor } from './auth/auth.interceptor';
import { LoginComponent } from './start-page/authorization/login/login.component';
import { RegistrationComponent } from './start-page/authorization/registration/registration.component';
import { HeaderComponent } from './shared/layout/header/header.component';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { MainPageBodyComponent } from './main-page-body/main-page-body.component';
import { NoaccsessComponent } from './noaccsess/noaccsess.component';
import { FeedFaqComponent } from './list-of-faq/feed-faq/feed-faq.component';
import { AboutSitePageComponent } from './about-site-page/about-site-page.component';
import { MarkdownModule, MarkedOptions, MarkedRenderer } from 'ngx-markdown';
import { PostsService} from './feed-posts/posts.service';
import { UserSortTableComponent } from './user-about-panel/user-sort-table/user-sort-table.component'




@NgModule({
  declarations: [
    AppComponent,
    StartPageComponent,
    PopularTagCloudComponent,
    CategorySelectorComponent,
    PostComponent,
    FormEditCommentComponent,
    CommentComponent,
    ListOfFaqComponent,
    AddPostFormComponent,
    AddPostButtonComponent,
    FeedPostsComponent,
    FooterComponent,
    UserAboutPanelComponent,
    LoginComponent,
    RegistrationComponent,
    HeaderComponent,
    MainPageBodyComponent,
    NoaccsessComponent,
    FeedFaqComponent,
    AboutSitePageComponent,
	  UserSortTableComponent
  ],
  imports: [
    BrowserModule,
    // RouterModule.forRoot(appRoutes),
    FormsModule,
    HttpClientModule,
    CommonModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot(),
    AppRoutingModule,
    LMarkdownEditorModule,
    MarkdownModule.forRoot(),
    ReactiveFormsModule
  ],
  providers: [PostsService, {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent],
})
export class AppModule { }
