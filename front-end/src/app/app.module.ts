import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AccountService } from "./services/account/account.service";
import { Http, HttpModule} from "@angular/http";
import { HttpClientModule } from "@angular/common/http";

import { ServicesModule } from "./services/services.module";
import { FormsModule } from "@angular/forms";

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { AppRoutingModule } from './shared/app-routing.module';
import { RegisterComponent } from './components/register/register.component';
import { PostsComponent } from './components/posts/posts.component';
import { CommentsComponent } from './components/comments/comments.component';
import { SinglePostComponent } from './components/single-post/single-post.component';
import { SingleCommentComponent } from './components/single-comment/single-comment.component';
import { CreateCommentComponent } from './components/create-comment/create-comment.component';
import { CreatePostComponent } from './components/create-post/create-post.component';
import { SearchComponent } from "./components/search/search.component";
import { NewsComponent } from './components/news/news.component';
import { HomeComponent } from './components/home/home.component';
import { HeaderComponent } from './shared/header/header.component';
import { FooterComponent } from './shared/footer/footer.component';
import { ShowAuthedDirective } from './shared/show-authed.directive';
import { PostsService } from './services/posts/posts.service';
import { AdminComponent } from './components/admin/admin.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    PostsComponent,
    CommentsComponent,
    SinglePostComponent,
    SingleCommentComponent,
    CreateCommentComponent,
    CreatePostComponent,
    SearchComponent,
    NewsComponent,
    HomeComponent,
    HeaderComponent,
    FooterComponent,
    ShowAuthedDirective,
    AdminComponent
  ],
  imports: [
    BrowserModule,
    ServicesModule,
    AppRoutingModule,
    FormsModule,
    HttpModule,
    HttpClientModule
  ],
  providers: [AccountService, PostsService],
  bootstrap: [AppComponent]
})
export class AppModule { }
