import {Component, OnInit, ViewChild} from '@angular/core';
import { AccountService } from '../start-page/account.service';
import { UserService } from '../user-about-panel/user.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PostService} from '../post/post.service';
import { HttpErrorResponse } from '@angular/common/http';



@Component({
  selector: 'app-add-post-form',
  templateUrl: './add-post-form.component.html',
  styleUrls: ['./add-post-form.component.sass']
})
export class AddPostFormComponent implements OnInit {
  currentUser;
  content: string;
  title: string;
  tags: string[] = [];


  constructor(
    private userService: UserService,
    private accountService: AccountService,
    private router: Router,
    private toastr: ToastrService,
    private postService: PostService
  ) { }

  ngOnInit() {
    this.accountService.GetUserProfile().subscribe(res => {
      this.currentUser = res;
  }
    ); }

  addPost() {
    this.postService.addPost(this.title, this.content, this.currentUser.id).subscribe((res: any) => {
      if (!(this.tags.length === 0 || undefined))
      {
        this.postService.addTags(this.tags).subscribe();
        this.postService.addTagsToPost(res.id, this.tags).subscribe( _ => {
          this.router.navigateByUrl('/posts');
          this.toastr.success('Пост успешно добавлен');
        });
      } else {
        this.toastr.success('Пост успешно добавлен');
        this.router.navigateByUrl('/posts');
      }
    }, (errorResponse: HttpErrorResponse) => {
      for (const err in errorResponse.error) {
          switch (err) {
            case('Title'):
              this.toastr.error('Добавьте заголовок поста');
              break;
            case('Content'):
              this.toastr.error('Пост не может быть пустым');
              break;
          }
        }
      });
    }

    addTag(newTag: string) {
      if (newTag.length > 16) {
        this.toastr.error('Слишком длинный тэг');
      } else if (newTag.length < 2) {
        this.toastr.error('Слишком короткий тэг');
      } else if (this.tags.includes(newTag)) {
        this.toastr.error('Тэг уже добавлен');
      } else if (this.tags.length > 5) {
        this.toastr.error('Превышен лимит тэгов');
      } else {
        this.tags.push(newTag);
      }
    }

    deleteTag(indexTag: number) {
      this.tags.splice(indexTag,1);
      console.log(this.tags.length);
    }
  }




