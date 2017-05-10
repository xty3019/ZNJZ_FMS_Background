本类库主要处理界面的逻辑，界面通过访问这个层拿到数据


界面访问顺序是：界面调用->BLL(Business Logic Layer)->DAL(Data Access Layer)
数据回返顺序是：DAL->BLL->界面