using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static clsMain;
using static MainClass;

    public class Category
    {
    
    //راجع المصدر المشروع بشكل موسع ل معرفه اذا اتحقق من ان اسم الكاتيقوري موجود
    //وبالتالي اصنع دالة داخلية او عامة للتحقق

        public int? ID { get; set; }
        public string CategoryName { get; set; }
        public string Color { get; set; }

        enSaveMode Mode = enSaveMode.AddNew;

    public Category(int? iD, string categoryName, string color)
    {
        ID = iD;
        CategoryName = categoryName;
        Color = color;
        Mode = enSaveMode.Update;
    }

    public Category()
    {
        ID = null;
        CategoryName = string.Empty;
        Color = string.Empty;
        Mode = enSaveMode.AddNew;
    }

        
        private bool _AddCategory()
        {
           this.ID =  ClsCategoriesDB.InsertCategory(this.CategoryName,this.Color);
            return this.ID != null;
        }

        private bool _UpdateCategory()
        {
           int result = ClsCategoriesDB.UpdateCategory(this.ID.Value, this.CategoryName, this.Color);
            return result > 0 ;
        }

        public Category Find(int CategoryID)
    {
        if (ClsCategoriesDB.GetCategoryByID(CategoryID,out string Name, out string Color))
        {
            return new Category(CategoryID, Name, Color);
        }
        return null;
    }

        public bool Save()
    {
        switch (Mode)
        {
            case enSaveMode.AddNew:
                if (_AddCategory())
                {

                    Mode = enSaveMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }

            case enSaveMode.Update:

                return _UpdateCategory();

        }

        return false;

    }

        public static bool DeleteCategory(int ID) => ClsCategoriesDB.DeleteCategory(ID);

        public static DataTable AllCategores() => ClsCategoriesDB.GetAllCategories();

      
        
    
    //دالة تحقق من ان التصنيف موجود
    //بعدين بنعرف اذا بتكون استاتيك او داخل الاوبجيكت او حتى داخل السيف

}

