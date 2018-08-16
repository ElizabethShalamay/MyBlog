import { Post } from "./post"

export class Blog {
    id: number;
    name: string;
    description: string;
    authorId: number;
    posts: Post[];
}