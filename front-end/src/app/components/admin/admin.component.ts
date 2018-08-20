import { Component, OnInit } from '@angular/core';
import { User } from "../../models/user";
import { Comment } from "../../models/comment";
import { Post } from "../../models/post";
import { UsersService } from '../../services/users/users.service';
import { PostsService } from '../../services/posts/posts.service';
import { CommentsService } from '../../services/comments/comments.service';
import { AccountService } from '../../services/account/account.service';
import { Router } from '../../../../node_modules/@angular/router';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  users: User[] = [];
  posts: Post[] = [];
  comments: Comment[] = [];

  usersTab: boolean = false;
  postsTab: boolean = false;
  commentsTab: boolean = false;

  user: User;
  post: Post;

  open: boolean;

  constructor(private userService: UsersService,
    private postService: PostsService,
    private commentService: CommentsService,
    private accService: AccountService,
    private router: Router) { }

  chooseTab(tab: string) {
    this.usersTab = tab === 'users';
    this.postsTab = tab === 'posts';
    this.commentsTab = tab === 'comments';
  }

  getUsers(page: number) {
    this.userService.getUsers(page, 'api/admin/users')
      .subscribe(data => this.users.push(...data));
  }

  getPosts(page: number) {
    this.postService.getPosts(page, 'api/admin/posts')
      .subscribe(data => this.posts.push(...data.body));
  }

  getComments(page: number) {
    this.commentService.getAllComments(page, 'api/admin/comments')
      .subscribe(data => this.comments.push(...data));
  }

  openUser(id: string) {
    this.user = new User();
    this.userService.getUser(id, 'api/admin/users').subscribe(user => {
      this.user = user;
      this.open = !this.open;
    });
  }

  openPost(id: number) {
    this.post = new Post();
    this.postService.getPost(id, 'api/admin/posts').subscribe(post => {
      this.post = post;
      this.open = !this.open;
    });
  }

  approvePost(id: number) {
    let post = this.posts.find(p => p.Id == id);
    post.IsApproved = true;
    this.postService.updatePost(post).subscribe();
  }

  deletePost(id: number) {
    let post = this.posts.find(p => p.Id == id);
    this.posts = this.posts.filter(p => p !== post);
    this.postService.removePost(post).subscribe();
  }

  approveComment(id: number) {
    let comment = this.comments.find(c => c.Id == id);
    comment.IsApproved = true;
    this.commentService.updateComment(comment).subscribe();
  }

  deleteComment(id: number) {
    let comment = this.comments.find(c => c.Id == id);
    this.comments = this.comments.filter(c => c !== comment);
    this.commentService.removeComment(comment).subscribe();
  }

  ngOnInit() {
    if (!this.accService.authentication.isAuth) {
      this.router.navigate(["/login"]);
    }
    let page = 1;
    this.getUsers(page);
    this.getPosts(page);
    this.getComments(page);
  }
}
