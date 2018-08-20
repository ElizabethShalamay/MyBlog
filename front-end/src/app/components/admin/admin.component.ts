import { Component, OnInit } from '@angular/core';
import { User } from "../../models/user";
import { Comment } from "../../models/comment";
import { Post } from "../../models/post";
import { UsersService } from '../../services/users/users.service';
import { PostsService } from '../../services/posts/posts.service';
import { CommentsService } from '../../services/comments/comments.service';

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

  constructor(private userService: UsersService,
    private postService: PostsService,
    private commentService: CommentsService) { }


  chooseTab(tab: string) {
    this.usersTab = tab === 'users';
    this.postsTab = tab === 'posts';
    this.commentsTab = tab === 'comments';
  }


  getUsers(page: number) {
    this.userService.getUsers(page, 'api/admin/users').subscribe(data => this.users.push(...data));
  }

  getPosts(page: number) {
    this.postService.getPosts(page, 'api/admin/posts').subscribe(data => this.posts.push(...data.body));
  }

  getComments(page: number) {
    this.commentService.getAllComments(page, 'api/admin/comments').subscribe(data => this.comments.push(...data));
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
    this.getUsers(1); /// ??????????????????????????
    this.getPosts(1);
    this.getComments(1);
  }
}
