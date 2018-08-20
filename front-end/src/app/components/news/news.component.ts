import { Component, OnInit } from '@angular/core';
import { News } from "../../models/news";
import { Tag } from "../../models/tag";
import { PostsService } from "../../services/posts/posts.service";
import { TagsService } from "../../services/tags/tags.service";
import { Router } from '../../../../node_modules/@angular/router';
import { PaginationInfo } from "../../models/pagination-data";
import { AccountService } from '../../services/account/account.service';

@Component({
  selector: 'app-news',
  templateUrl: './news.component.html',
  styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit {

  news: News[] = [];
  tags: Tag[] = [];
  page = 1;
  paginationInfo: PaginationInfo = new PaginationInfo();

  constructor(private postsService: PostsService,
    private tagsService: TagsService,
    private router: Router,
    private accService: AccountService) { }

  getNews() {
    this.news = [];
    this.paginationInfo = new PaginationInfo();

    this.postsService.getNews(this.page).subscribe(
      data => {
        this.news.push(...data.body["posts"]);
        this.paginationInfo = JSON.parse(data.body["pagination_info"]);
      }
    );
  }

  getPostsByUser(userId: string) {
    this.postsService.getPostsByAuthor(1, userId).subscribe(
      data => {
        this.postsService.posts.push(...data);
        this.router.navigate(["search"]);
      }
    )
  }

  goNext() {
    if (this.paginationInfo.nextPage) {
      this.page++;
      this.getNews();
    }
  }

  goPrev() {
    if (this.paginationInfo.previousPage) {
      this.page--;
      this.getNews();
    }
  }

  getTags() {
    this.tagsService.getAll().subscribe(
      tags => {
        this.tags.push(...tags);
      }
    )
  }

  onTagClick(tag: string) {
    let tagString = `#${tag}`;

    this.postsService.search(tagString).subscribe(data =>
      this.postsService.posts.push(...data)
    );
    this.router.navigate(['search']);
  }

  ngOnInit() {
    if (!this.accService.authentication.isAuth) {
      this.router.navigate(["/login"]);
    }
    this.getNews();
    this.getTags();
  }
}
