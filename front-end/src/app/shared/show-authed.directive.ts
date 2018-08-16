import {
    Directive,
    Input,
    OnInit,
    TemplateRef,
    ViewContainerRef,
  } from '@angular/core';
  
  import { AccountService } from '../services/account/account.service';
  
  @Directive({ selector: '[appShowAuthed]' })
  export class ShowAuthedDirective implements OnInit {
    constructor(
      private templateRef: TemplateRef<any>,
      private accService: AccountService,
      private viewContainer: ViewContainerRef
    ) {}
  
    @Input() isAuthenticated = this.accService.authentication.isAuth;
    condition: boolean;
  
    ngOnInit() {
        
          if (this.isAuthenticated && this.condition || !this.isAuthenticated && !this.condition) {
            this.viewContainer.createEmbeddedView(this.templateRef);
          } else {
            this.viewContainer.clear();
          }
        }

    @Input() set appShowAuthed(condition: boolean) {
      this.condition = condition;
    }
}