﻿using AI_Sports.Dao;
using AI_Sports.Entity;
using AI_Sports.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Sports.Dao
{
    /// <summary>
    /// 会员DAO-CQZ
    /// </summary>
    class MemberDAO : BaseDAO<MemberEntity>
    {
        /// <summary>
        /// 新增会员插入基本信息，不包括卡号
        /// </summary>
        /// <param name="memberEntity"></param>
        /// <returns></returns>
        //public int InsertMember(MemberEntity memberEntity)
        //{
        //    using (var conn = DbUtil.getConn())
        //    {
        //        const string insert = "INSERT INTO bdl_member (`member_firstName`, `member_familyName`, `birth_date`, `sex`, `address`, `email_address`, `work_phone`, `personal_phone`, `mobile_phone`, `weight`, `height`, `age`, `max_heart_rate`, `suitable_heart_rate`, `role_id`, `fk_coach_id`, `label_name`, `remark`) VALUES (@Member_firstName, @Member_familyName, @Birth_date, @Sex, @Address, @Email_address, @Work_phone, @Personal_phone, @Mobile_phone, @Weight, @Height, @Age, @Max_heart_rate, @Suitable_heart_rate, @Role_id, @Fk_coach_id, @Label_name, @Remark)";

        //        return conn.Execute(insert, memberEntity);

        //    }
        //}

        /// <summary>
        /// 根据会员ID更新减脂模式是否开启
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="enable"></param>
        public void UpdateDeFatState(string memberId, bool enable)
        {
            using (var conn = DbUtil.getConn())
            {
                const string sql = "update bdl_member set is_open_fat_reduction = @DefatEnable where member_id = @MemberId";

                conn.Execute(sql, new { DefatEnable = enable, MemberId = memberId });

            }
        }

		/// <summary>
		/// 更新memberId
		/// </summary>
		/// <param name="oldMemberId"></param>
		/// <param name="newMemberId"></param>
		public void UpdateMemberId(string oldMemberId,string  newMemberId)
		{
			using (var conn = DbUtil.getConn())
			{
				const string sql = "update bdl_member set member_id = @newMemberId where member_id = @oldMemberId";

				conn.Execute(sql, new { newMemberId = newMemberId, oldMemberId = oldMemberId });

			}
		}
        ///cnk
        /// <summary>
        /// 根据memberid查询bdl_member中的数据主键id,主要用于上传数据
        /// </summary>
        /// <returns></returns>
        public List<long> ListIdByMemberId(string memberId)
        {
            using (var conn = DbUtil.getConn())
            {

                const string query = "SELECT id FROM bdl_member WHERE member_id = @member_id";

                return (List<long>)conn.Query<long>(query, new { member_id = memberId });

            }
        }
        /// <summary>
        /// 根据会员卡号查询会员基本信息
        /// </summary>
        /// <param name="memberEntity"></param>
        /// <returns></returns>
        public MemberEntity GetMember(string memberId)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT * FROM bdl_member WHERE member_id = @memberId";
                return conn.QueryFirstOrDefault<MemberEntity>(query, new { memberId });
            }
        }
       

        /// <summary>
        /// 根据教练ID查出其对应的会员集合
        /// </summary>
        /// <param name="memberEntity"></param>
        /// <returns></returns>
        public List<MemberEntity> listMemberByCoachId(MemberEntity memberEntity)
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT * FROM bdl_member WHERE fk_coach_id = @Fk_coach_id";
                return conn.Query<MemberEntity>(query, new { Fk_coach_id = memberEntity.Fk_coach_id }).ToList();
            }
        }
        /// <summary>
        /// 查询7天内登陆的活跃会员
        /// </summary>
        /// <returns></returns>
        public List<MemberEntity> ListActiveMember()
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT * FROM bdl_member WHERE DATEDIFF(CURDATE(),last_login_date) > 0 AND DATEDIFF(CURDATE(),last_login_date) < 7";
                return conn.Query<MemberEntity>(query).ToList();
            }
        }
        /// <summary>
        /// 查询大于7天未登录的不活跃会员
        /// </summary>
        /// <returns></returns>
        public List<MemberEntity> ListInactiveMember()
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT * FROM bdl_member WHERE DATEDIFF(CURDATE(),last_login_date) > 7";
                return conn.Query<MemberEntity>(query).ToList();
            }
        }
                /// <summary>
        /// 查询所有会员信息，暂时用于search.xaml.cs中
        /// </summary>
        /// <returns></returns>
        public List<MemberEntity> ListAllMember()
        {
            using (var conn = DbUtil.getConn())
            {
                const string query = "SELECT * FROM bdl_member";
                return conn.Query<MemberEntity>(query).ToList();
            }
        }

        /// <summary>
        /// 模糊查询名字手机号，暂时用于search.xaml.cs中
        /// </summary>
        /// <returns></returns>
        public List<MemberEntity> ListNameMember(String firstName,String phone_Number)
        {
            //mysql语句
            string query=null;
            using (var conn = DbUtil.getConn())
            {

                //四种情况判断
                if((firstName == "会员名" || firstName == "" || firstName == null)&&(phone_Number == "手机号" || phone_Number == "" || phone_Number == null))
                {
                    query = "SELECT * FROM bdl_member";
                }
                else
                
                    if ((firstName == "会员名" || firstName == "" || firstName == null) && (phone_Number != "手机号" && phone_Number != "" && phone_Number != null))
                    {
                        query = "select * from bdl_member where mobile_phone like '%"+phone_Number+"%'";
                    }
                else
                    if ((firstName != "会员名" && firstName != "" && firstName != null) && (phone_Number == "手机号" || phone_Number == "" || phone_Number == null))
                    {
                        query = "select * from bdl_member where member_firstName like '%" + firstName + "%'";
                    }
                else
                    if ((firstName != "会员名" && firstName != "" && firstName != null) && (phone_Number != "手机号" && phone_Number != "" && phone_Number != null))
                {
                    query = "select * from bdl_member where member_firstName like '%" + firstName + "%' and mobile_phone like '%" + phone_Number + "%'";
                }

                
                return conn.Query<MemberEntity>(query).ToList();
            }
        }
       
    }


}
