import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { Post } from "../../models/post";
import { PostsService } from '../../services/posts/posts.service';
import { AccountService } from '../../services/account/account.service';
import { Router } from '../../../../node_modules/@angular/router';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit, AfterViewChecked {
  posts: Post[] = [];
  currentPage: Post[] = [];
  page = 1;
  pageSize = 5;

  constructor(private postsService: PostsService,
    private accService: AccountService,
    private router: Router) { }

  ngOnInit() {
    if (!this.accService.authentication.isAuth) {
      this.router.navigate(["/login"]);
    }
    this.posts = this.postsService.posts;
  }

  goNext() {
    let start = (this.page - 1) * this.pageSize;

    if (this.page < Math.ceil(this.posts.length / this.pageSize)
      && start + this.pageSize < this.posts.length) {
      this.page++;
      this.currentPage.push(...this.posts.slice(start, start + this.pageSize));
    }

    if (this.page < Math.ceil(this.posts.length / this.pageSize)
      && start + this.pageSize > this.posts.length) {
      this.page++;
      this.currentPage.push(...this.posts.slice(start));
    }
  }

  goPrev() {
    let start = (this.page - 1) * this.pageSize;
    if (this.page > 1) {
      this.page--;
      this.currentPage.push(...this.posts.slice(start, start + this.pageSize));
    }
  }

  ngAfterViewChecked() {
    if (this.posts != this.postsService.posts)
      this.posts = this.postsService.posts;
  }
}
