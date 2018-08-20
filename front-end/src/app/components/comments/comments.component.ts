import { Component, Input, OnInit } from '@angular/core';
import { Comment } from "../../models/comment";
import { CommentsService } from "../../services/comments/comments.service";
import { Router, ActivatedRoute } from "@angular/router";
import { AccountService } from '../../services/account/account.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  @Input() comments: Comment[] = [];
  seed: number;
  page = 1;

  constructor(private commentsService: CommentsService,
    private route: ActivatedRoute,
    private router: Router,
    private accService: AccountService) { }

  getComments() {
    if (!this.comments.length) {
      const id = +this.route.snapshot.paramMap.get('id');
      this.commentsService.getComments(id, this.page).subscribe(
        data => {
          this.comments.push(...data);
          this.page++;
        });
    }
  }

  ngOnInit() {
    if (!this.accService.authentication.isAuth) {
      this.router.navigate(["/login"]);
    }
    this.getComments();
  }
}
