import { Component, OnInit, Input } from '@angular/core';
import { AccountService } from '../../services/account/account.service';
import { PostsService } from "../../services/posts/posts.service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  isAuthenticated: boolean;
  search: string;

  constructor(private accService: AccountService,
    private postService: PostsService,
    private router: Router) {
    accService.getLoggedIn.subscribe(auth => this.setAuth(auth));
  }


  onSearchClick() {
    this.postService.search(this.search).subscribe(data => {
      this.postService.posts.push(...data);
      this.search = "";
    }
    );
    this.router.navigate(['search']);
  }
  onLogOffClick() {
    this.accService.logOut();
  }

  private setAuth(auth: boolean): void {
    this.isAuthenticated = auth;
  }

  ngOnInit() {
  }

}
