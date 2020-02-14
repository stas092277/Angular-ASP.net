import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { AccountService } from 'src/app/start-page/account.service';
import { CategorySortService } from './category-sort.service';
import { Post } from 'src/app/post/Itpost';


@Component({
  selector: 'app-category-selector',
  templateUrl: './category-selector.component.html',
  styleUrls: ['./category-selector.component.sass']
})
export class CategorySelectorComponent implements OnInit {

  postList: Post[];
  categorys = CATEGORYS;
  selectedCategory = CATEGORYS[1];
  @Output() outPostLost = new EventEmitter();

  constructor(
    private accountService: AccountService,
    private categrySortService: CategorySortService
  ) { }

  ngOnInit() {
  }

  onSelect(category: Category): void {
    this.selectedCategory = category;

    switch(category.name) {
      case 'Лучшее' : {
        this.categrySortService.getDataTopPosts().subscribe(data =>{
          this.postList = data;
          this.outPostLost.emit(this.postList);

        },
          err => console.log('HTTP error', err.message));
        break;
      }
      case 'Полезное': {
         this.categrySortService.getDataUsefulPosts().subscribe(data =>{
          this.postList = data;
          this.outPostLost.emit(this.postList);
        },
          err => console.log('HTTP error', err.message));
         break;
      }
      default: {
        this.categrySortService.getDataAllPosts().subscribe(data =>{
          this.postList = data;
          this.outPostLost.emit(this.postList);
        },
          err => console.log('HTTP error', err.message));
        break;
      }
   }
  }
}

class Category {
  name: string;
}

const CATEGORYS: Category[] =
  [
    { name: 'Лучшее' },
    { name: 'Новое' },
    { name: 'Полезное' }
  ];
