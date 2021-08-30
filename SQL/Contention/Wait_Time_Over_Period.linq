<Query Kind="SQL" />

/* Snapshot the current wait stats and store so that this can be compared over a time period 
   Return the statistics between this point in time and the last collection point in time.
   
   **This data is maintained in tempdb so the connection must persist between each execution**
   **alternatively this could be modified to use a persisted table in tempdb.  if that
   is changed code should be included to clean up the table at some point.**
*/
use tempdb
go

declare @current_snap_time datetime;
declare @previous_snap_time datetime;

set @current_snap_time = GETDATE();

if not exists(select name from tempdb.sys.sysobjects where name like '#_wait_stats%')
   create table #_wait_stats
   (
	  wait_type varchar(128)
      ,waiting_tasks_count bigint
	  , wait_time_ms bigint
      ,avg_wait_time_ms int
      ,max_wait_time_ms bigint
	  , signal_wait_time_ms bigint
      ,avg_signal_wait_time int
      ,snap_time datetime
   );

insert into #_wait_stats (
         wait_type
         ,waiting_tasks_count
         ,wait_time_ms
         ,max_wait_time_ms
         ,signal_wait_time_ms
         ,snap_time
      )
      select
		 wait_type
		 , waiting_tasks_count
		 , wait_time_ms
		 , max_wait_time_ms
		 , signal_wait_time_ms
		 ,getdate()

	  from sys.dm_os_wait_stats;

/* get the previous collection point */
select top 1 @previous_snap_time = snap_time from #_wait_stats 
         where snap_time < (select max(snap_time) from #_wait_stats)
         order by snap_time desc;

/* get delta in the wait stats */
select top 10

	  s.wait_type
      , (e.waiting_tasks_count - s.waiting_tasks_count) as [waiting_tasks_count]
      , (e.wait_time_ms - s.wait_time_ms) as [wait_time_ms]
      , (e.wait_time_ms - s.wait_time_ms) / ((e.waiting_tasks_count - s.waiting_tasks_count)) as [avg_wait_time_ms]
      , (e.max_wait_time_ms) as [max_wait_time_ms]
      , (e.signal_wait_time_ms - s.signal_wait_time_ms) as [signal_wait_time_ms]
      , (e.signal_wait_time_ms - s.signal_wait_time_ms) / ((e.waiting_tasks_count - s.waiting_tasks_count)) as [avg_signal_time_ms]
      , s.snap_time as [start_time]
      , e.snap_time as [end_time]
      , DATEDIFF(ss, s.snap_time, e.snap_time) as [seconds_in_sample]
   from #_wait_stats e
   inner join (

	  select *from #_wait_stats 
         where snap_time = @previous_snap_time
      ) s on(s.wait_type = e.wait_type)
   where
	  e.snap_time = @current_snap_time

	  and s.snap_time = @previous_snap_time

	  and e.wait_time_ms > 0 

	  and (e.waiting_tasks_count - s.waiting_tasks_count) > 0 

	  and e.wait_type NOT IN ('LAZYWRITER_SLEEP', 'SQLTRACE_BUFFER_FLUSH'
                              , 'SOS_SCHEDULER_YIELD','DBMIRRORING_CMD', 'BROKER_TASK_STOP'
                              , 'CLR_AUTO_EVENT', 'BROKER_RECEIVE_WAITFOR', 'WAITFOR'
                              , 'SLEEP_TASK', 'REQUEST_FOR_DEADLOCK_SEARCH', 'XE_TIMER_EVENT'
                              , 'FT_IFTS_SCHEDULER_IDLE_WAIT', 'BROKER_TO_FLUSH', 'XE_DISPATCHER_WAIT'
                              , 'SQLTRACE_INCREMENTAL_FLUSH_SLEEP')

order by (e.wait_time_ms - s.wait_time_ms) desc;

/* clean up table */
delete from #_wait_stats
where snap_time = @previous_snap_time;