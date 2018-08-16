export class Comment {
    AuthorName: string;
    Text: string;
    Date: Date;
    Id: number;
    PostId: number;
    ParentId: number;
    Answer: Boolean;
    IsApproved: Boolean;
    Children: Comment[];
}