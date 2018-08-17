import { Injectable } from '@angular/core';
import { Post } from "../../models/post";
import { Observable } from 'rxjs';
import { Router } from "@angular/router";
import { News } from "../../models/news";
import { AccountService } from "../account/account.service";
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { catchError, map, tap } from "rxjs/operators";
import { CommentsService } from '../comments/comments.service';
import { PaginationInfo } from "../../models/pagination-data";

@Injectable({
  providedIn: 'root'
})
export class PostsService {

  baseUrl: string = "api/posts";
  searchUrl: string = "api/search";
  posts: Post[] = [];
  paginationInfo: PaginationInfo;

  constructor(private router: Router,
    private accService: AccountService,
    private httpClient: HttpClient) { }

  getPost(id: number): Observable<Post> {
    const url = `${this.baseUrl}/${id}`;
    return this.httpClient.get<Post>(url, { headers: this.accService.getAuthHeaders() });
  }

  getPosts(page: number, url: string = ""): Observable<HttpResponse<Post[]>> {
    url = url ? `${url}?page=${page}` : `${this.baseUrl}?page=${page}`;
    return this.httpClient.get<Post[]>(url, { headers: this.accService.getAuthHeaders(), observe: 'response' });
  }

  getNews(): Observable<News[]> {
    const url = `${this.baseUrl}/news`;
    return this.httpClient.get<News[]>(url, { headers: this.accService.getAuthHeaders() });
  }

  getPostsByAuthor(page: number, authorId: string): Observable<any> {
    const url = `${this.baseUrl}?page=${page}&authorId=${authorId}`;
    return this.httpClient.get<any>(url, { headers: this.accService.getAuthHeaders() });
  }

  search(text: string): Observable<Post[]> {
    this.posts = [];
    text = encodeURIComponent(text);

    let searchTag = text.replace(encodeURIComponent("#"), "");
    const url = text.startsWith(encodeURIComponent("#")) ?
      `${this.searchUrl}/tag?tag=${searchTag}` :
      `${this.searchUrl}/text?text=${text}`;
    return this.httpClient.get<Post[]>(url, { headers: this.accService.getAuthHeaders() });
  }

  addPost(post: Post): Observable<Post> {
    post.PostedAt = new Date();
    return this.httpClient.post<Post>
      (this.baseUrl, post, { headers: this.accService.getAuthHeaders() })
      .pipe(
        tap(() => {
          this.router.navigate(['/posts']);
        })
      );
  }
  updatePost(post: Post): Observable<any> {
    let url = `${this.baseUrl}/${post.Id}`;
    return this.httpClient.put(url, post, { headers: this.accService.getAuthHeaders() });
  }
  removePost(post: Post): Observable<Post> {
    return this.httpClient.delete<Post>
      (`${this.baseUrl}/${post.Id}`, { headers: this.accService.getAuthHeaders() });
  }
}