import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../services/account/account.service';
import { PostsService } from "../../services/posts/posts.service";
import { Router } from "@angular/router";
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  title = 'MyBlog';
  search: string;
  isAuthenticated: boolean;

  constructor(private accService: AccountService,
    private postService: PostsService,
    private router: Router) {
    accService.getLoggedIn.subscribe(auth => this.setAuth(auth));
  }

  onSearchClick() {
    this.postService.search(this.search).subscribe(data => this.postService.posts = data);
    this.router.navigate['/search'];
  }

  onLogOffClick() {
    this.accService.logOut();
  }

  private setAuth(auth: boolean): void {
    this.isAuthenticated = auth;
  }

  ngOnInit() {
    if (!this.accService.authentication.isAuth) {
      this.router.navigate(["/login"]);
    }
  }
}