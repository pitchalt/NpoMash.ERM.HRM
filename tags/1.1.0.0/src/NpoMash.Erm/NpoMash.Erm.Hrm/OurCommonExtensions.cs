using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
//
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

/*public static class OneToOneRelationship<TYPE_OF_CURRENT_OBJECT, TYPE_OF_SECOND_OBJECT>
    where TYPE_OF_CURRENT_OBJECT : class
    where TYPE_OF_SECOND_OBJECT : class {
    public static void linkTwoObjects(
        TYPE_OF_SECOND_OBJECT ref_in_current_object,
        TYPE_OF_CURRENT_OBJECT ref_in_second_object,
        TYPE_OF_CURRENT_OBJECT ref_in_old_second_object) {
            TYPE_OF_SECOND_OBJECT old_value = ref_in_current_object;
            if (ref_in_second_object == old_value) return;
            else if (
    }
}*/