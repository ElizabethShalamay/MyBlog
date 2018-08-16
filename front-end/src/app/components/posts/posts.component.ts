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
  paginationInfo: PaginationInfo;
  headers: HttpHeaders;

  getPosts() {
    this.posts = [];
    this.postsService.getPosts(this.page).subscribe(
      (data) => {
        this.posts.push(...data.body);
        this.paginationInfo = JSON.parse(data.headers.get('paging-headers'));
        console.log(this.paginationInfo);
      }
    );
  }

  goNext() {
    if (this.paginationInfo.nextPage == 'Yes') {
      this.page++;
      this.getPosts();
    }
  }
  goPrev() {
    if (this.paginationInfo.previousPage == 'Yes') {
      this.page--;
      this.getPosts();
    }
  }

  search(search: string) {
    this.postsService.search(`#${search}`).subscribe(data =>
      this.postsService.posts.push(...data)
    );
    this.router.navigate(['search']);
  }
  getPostsByAuthor(authorId: string) {
    this.postsService.getPostsByAuthor(this.page, authorId).subscribe(
      data => {
        this.posts.push(...data);
        this.page++;
      }
    );
  }

  getPost(id: number) {
    this.postsService.getPost(id).subscribe(
      data => {
        this.post = data;
      }
    );
  }

  ngOnInit() {
    if (this.accService.authentication.isAuth) {
      this.getPosts();
    }
    else {
      this.router.navigate(['/login']);
    }
  }

}
