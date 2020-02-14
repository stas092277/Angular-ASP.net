import { comment } from './comment/Itcomment';

export class Post {
  id: number;
  authorId: number;
  publishDate: string;
  title: string;
  content: string;
  likesCount: number;
  savesCount: number;
  commentsCount: number;
  author;
  tags: any;
  comments: comment;

  liked: boolean;
  saved: boolean;


}
