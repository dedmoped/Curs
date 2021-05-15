import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecentlylookComponent } from './recentlylook.component';

describe('RecentlylookComponent', () => {
  let component: RecentlylookComponent;
  let fixture: ComponentFixture<RecentlylookComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecentlylookComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RecentlylookComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
