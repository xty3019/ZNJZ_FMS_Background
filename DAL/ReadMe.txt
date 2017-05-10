本类库主要负责从数据库获取数据，将数据返回给逻辑层（BLL）或外部库（HD.Service)使用

界面访问顺序是：界面调用->BLL(Business Logic Layer)->DAL(Data Access Layer)
数据回返顺序是：DAL->BLL->界面