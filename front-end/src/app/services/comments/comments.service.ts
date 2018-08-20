import { Injectable } from '@angular/core';
import { Comment } from "../../models/comment";
import { Observable } from 'rxjs';
import { Router } from "@angular/router";
import { AccountService } from "../account/account.service";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from "rxjs/operators";
import { News } from '../../models/news';


@Injectable({
  providedIn: 'root'
})
export class CommentsService {

  baseUrl: string = "api/comments";

  constructor(private router: Router,
    private accService: AccountService,
    private httpClient: HttpClient) { }

  getAllComments(page: number, url: string = ""): Observable<Comment[]> {
    url == "" ? `${this.baseUrl}?page=${page}` : `${url}?page=${page}`;
    return this.httpClient.get<Comment[]>(url, { headers: this.accService.getAuthHeaders() });
  }
  getComments(postId: number, page: number): Observable<Comment[]> {
    const url = `${this.baseUrl}/${postId}?page=${page}`;
    return this.httpClient.get<Comment[]>(url, { headers: this.accService.getAuthHeaders() });
  }

  getComment(id: number): Observable<Comment> {
    const url = `${this.baseUrl}/${id}`;
    return this.httpClient.get<Comment>(url, { headers: this.accService.getAuthHeaders() });
  }

  addComment(postId: number, text: string, parentId?: number): Observable<Comment> {
    const comment: Comment = {
      Id: 0,
      AuthorName: "",
      Text: text,
      Date: new Date(),
      PostId: postId,
      ParentId: parentId,
      IsApproved: false,
      Answer: !!parentId,
      Children: []
    };
    const url = `${this.baseUrl}/${postId}`;
    return this.httpClient.post<Comment>(url, comment, { headers: this.accService.getAuthHeaders() })
      .pipe(
        tap(() => {
          if (this.accService.authentication.userName != "admin")
            this.router.navigate([`posts/${postId}`]);
        })
      );
  }
  updateComment(comment: Comment): Observable<any> {
    let url = `${this.baseUrl}/${comment.Id}`;
    return this.httpClient.put(url, comment, { headers: this.accService.getAuthHeaders() })
      .pipe(
        tap(() => {
          if (this.accService.authentication.userName != "admin")
            this.router.navigate([`posts/${comment.PostId}`]);
        })
      );
  }
  removeComment(comment: Comment): Observable<Comment> {
    return this.httpClient.delete<Comment>
      (`${this.baseUrl}?id=${comment.Id}`, { headers: this.accService.getAuthHeaders() }).pipe(
        tap(() => {
          if (this.accService.authentication.userName != "admin")
            this.router.navigate([`posts/${comment.PostId}`]);
        })
      );
  }
}
