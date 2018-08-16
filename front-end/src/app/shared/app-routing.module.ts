import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { RegisterComponent } from "../components/register/register.component";
import { LoginComponent } from "../components/login/login.component";
import { PostsComponent } from "../components/posts/posts.component";
import { SinglePostComponent } from "../components/single-post/single-post.component";
import { CreatePostComponent } from "../components/create-post/create-post.component";
import { SearchComponent } from "../components/search/search.component";
import { AdminComponent } from '../components/admin/admin.component';

const routes: Routes = [
  { path: '', component: PostsComponent },
  { path: "register", component: RegisterComponent },
  { path: "login", component: LoginComponent },
  { path: "posts", component: PostsComponent },
  { path: 'posts/:id', component: SinglePostComponent, runGuardsAndResolvers: 'always' },
  { path: "creator", component: CreatePostComponent },
  { path: "creator/:id", component: CreatePostComponent },
  { path: "search", component: SearchComponent },
  { path: "admin", component: AdminComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' })],
  exports: [RouterModule]
})

export class AppRoutingModule { }