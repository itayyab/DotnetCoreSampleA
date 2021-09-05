import { DebugElement } from '@angular/core';
import { ComponentFixture, inject, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { HttpErrorHandler } from '../http-error-handler.service';
import { Categories } from './Categories';

import { CategoriesService } from './categories.service';


let httpClientSpy: { get: jasmine.Spy };
let heroService: CategoriesService;

describe('CategoriesServiceC', () => {

  // let fixture: ComponentFixture<CategoriesService>;
  // let debugElement: DebugElement;
  // let incrementDecrementService: HttpErrorHandler;
  // let incrementSpy: any;
  // let     component :any;


  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [
        CategoriesService
      ],
      providers: [ HttpErrorHandler ]
    }).compileComponents();

    // fixture = TestBed.createComponent(CategoriesService);
    // debugElement = fixture.debugElement;
    // component = fixture.componentInstance;
    // fixture.detectChanges();
    // incrementDecrementService = debugElement.injector.get(HttpErrorHandler);
   // incrementSpy = spyOn(incrementDecrementService, 'increment').and.callThrough();
  });

  /*beforeEach(() => {
    TestBed.configureTestingModule({});
   // const aboutService = jasmine.createSpyObj('HttpErrorHandler', ['createHandleError','handleError']);
    const service = autoSpy(HttpErrorHandler);

    httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);
    service.createHandleError('CategoriesService');
    heroService = new CategoriesService(httpClientSpy as any, service, null);
});*/

// it('should return expected heroes (HttpClient called once)', (done: DoneFn) => {
//   const expectedHeroes: Categories[] =
//     [{ cat_id: 1, cat_name: 'A' }, { cat_id: 2, cat_name: 'B' }];

//   httpClientSpy.get.and.returnValue(of(expectedHeroes));

//   component.getHeroes().subscribe(
//     heroes => {
//       expect(heroes).toEqual(expectedHeroes, 'expected heroes');
//       done();
//     },
//     done.fail
//   );
//   expect(httpClientSpy.get.calls.count()).toBe(1, 'one call');
// });

it('should exist', inject([HttpErrorHandler], (loggingService : HttpErrorHandler) => {
  expect(loggingService).toBeDefined();
}));

});



describe('CategoriesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CategoriesService = TestBed.get(CategoriesService);
    expect(service).toBeTruthy();
  });

});

export type SpyOf<T> = T & { [k in keyof T]: jasmine.Spy };

export function autoSpy<T>(obj: new (...args: any[]) => T): SpyOf<T> {
  const res: SpyOf<T> = {} as any;

      const keys = Object.getOwnPropertyNames(obj.prototype);
      keys.forEach((key) => {
        res[key] = jasmine.createSpy(key);
      });

      return res;
}


