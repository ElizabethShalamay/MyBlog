import { Component,OnInit, AfterViewChecked } from '@angular/core';
import { Post } from "../../models/post";
import { PostsService } from '../../services/posts/posts.service';
import { PaginationInfo } from "../../models/pagination-data";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit, AfterViewChecked {
  posts: Post[] = [];

  currentPage: Post[] = [];

  page = 1;

  constructor(
    private postsService: PostsService
  ) { }

  ngOnInit() {
    this.posts = this.postsService.posts;
  }
  goNext() { 
    let start = (this.page-1)*5;
    if (this.page < Math.ceil(this.posts.length / 5) && start + 5 < this.posts.length) {
      this.page++;
      this.currentPage.push(...this.posts.slice( start, start + 5));
    }
    if (this.page < Math.ceil(this.posts.length / 5) && start + 5 > this.posts.length) {
      this.page++;
      this.currentPage.push(...this.posts.slice(start));
    }
  }

  goPrev() {
    let start = (this.page-1)*5;
    if (this.page > 1) {
      this.page--;
      this.currentPage.push(...this.posts.slice(start, start + 5));
    }
  }

  ngAfterViewChecked() {
    if (this.posts != this.postsService.posts)
      this.posts = this.postsService.posts;     
  }
}
