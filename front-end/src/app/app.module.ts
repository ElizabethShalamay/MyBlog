import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './shared/app-routing.module';
import { HttpModule} from "@angular/http";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule } from "@angular/forms";
import { ServicesModule } from "./services/services.module";

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AdminComponent } from './components/admin/admin.component';
import { HomeComponent } from './components/home/home.component';
import { HeaderComponent } from './shared/header/header.component';

import { PostsComponent } from './components/posts/posts.component';
import { CommentsComponent } from './components/comments/comments.component';
import { NewsComponent } from './components/news/news.component';

import { SinglePostComponent } from './components/single-post/single-post.component';
import { SingleCommentComponent } from './components/single-comment/single-comment.component';

import { CreatePostComponent } from './components/create-post/create-post.component';
import { CreateCommentComponent } from './components/create-comment/create-comment.component';

import { SearchComponent } from "./components/search/search.component";

import { PostsService } from './services/posts/posts.service';
import { AccountService } from "./services/account/account.service";

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    AdminComponent,
    HomeComponent,
    HeaderComponent,

    PostsComponent,
    CommentsComponent,
    NewsComponent,

    SinglePostComponent,
    SingleCommentComponent,

    CreateCommentComponent,
    CreatePostComponent,
    
    SearchComponent
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
