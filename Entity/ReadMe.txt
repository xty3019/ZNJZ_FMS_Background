本类库主要封装数据库表，将表中的记录变成一个实体类(Entity)返回

界面访问顺序是：界面调用->BLL(Business Logic Layer)->DAL(Data Access Layer)
数据回返顺序是：DAL->BLL->界面