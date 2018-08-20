import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { AccountService } from "../../services/account/account.service";
import { Post } from "../../models/post";
import { PostsService } from '../../services/posts/posts.service';
import { PaginationInfo } from "../../models/pagination-data";
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.css']
})
export class PostsComponent implements OnInit {

  constructor(private router: Router,
    private accService: AccountService,
    private postsService: PostsService) { }

  posts: Post[] = [];
  post: Post;
  page = 1;
  paginationInfo: PaginationInfo = new PaginationInfo();
  headers: HttpHeaders;

  getPosts() {
    this.posts = [];
    this.paginationInfo = new PaginationInfo();
    this.postsService.getPosts(this.page).subscribe(
      data => {
        this.posts.push(...data.body["posts"]);
        this.paginationInfo = JSON.parse(data.body["pagination_info"]);
      });
  }

  goNext() {
    if (this.paginationInfo.nextPage) {
      this.page++;
      this.getPosts();
    }
  }

  goPrev() {
    if (this.paginationInfo.previousPage) {
      this.page--;
      this.getPosts();
    }
  }

  search(search: string) {
    this.postsService.search(`#${search}`).subscribe(data => this.postsService.posts.push(...data));
    this.router.navigate(['search']);
  }

  getPostsByAuthor(authorId: string) {
    this.postsService.getPostsByAuthor(this.page, authorId).subscribe(
      data => {
        this.posts.push(...data);
        this.page++;
      });
  }

  getPost(id: number) {
    this.postsService.getPost(id).subscribe(data => this.post = data);
  }

  ngOnInit() {

    if (sessionStorage.getItem("Authorization")) {
      this.accService.authentication.isAuth = true;
      this.getPosts();
    }

    this.router.navigate(['/login']);
  }
}
